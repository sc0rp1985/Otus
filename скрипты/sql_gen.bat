@echo off
setlocal enabledelayedexpansion

:: Открытие выходного файла
echo INSERT INTO public.users(first_name, second_name, birthdate, city) VALUES > output.sql

:: Чтение входного CSV-файла построчно
set "linecount=0"
for /f "usebackq tokens=*" %%A in ("people.v2 (1).txt") do (

    echo Обрабатывается: %%A

    :: Извлекаем ФИО, дату и город
    for /f "tokens=1,2,3 delims=," %%B in ("%%A") do (

        :: ФИО разделяем на фамилию и имя
        for /f "tokens=1,2 delims= " %%F in ("%%B") do (

            set "first_name=%%G"
            set "second_name=%%F"
            set "birthdate=%%C"
            set "city=%%D"

            :: Формируем SQL-строку
            echo (!first_name!, !second_name!, !birthdate!, !city!) >> tmp_output.txt

            set /a linecount+=1
        )
    )
)

:: Теперь сформируем строки INSERT с закрывающими скобками и запятыми
setlocal enabledelayedexpansion
set "currentline=0"

(for /f "usebackq tokens=*" %%L in (tmp_output.txt) do (
    set /a currentline+=1

    :: Добавляем запятую к строке, кроме последней
    if !currentline! lss !linecount! (
        echo (%%L),
    ) else (
        echo (%%L);
    )
)) >> output.sql

:: Чистим временный файл
del tmp_output.txt

echo SQL файл создан: output.sql
pause
