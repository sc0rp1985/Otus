# Входной файл
$inputFile = "TextFile1.txt"

# Выходной файл
$outputFile = "output.sql"

# Записываем начало SQL
"INSERT INTO public.users(first_name, second_name, birthdate, city) VALUES" | Out-File -FilePath $outputFile -Encoding UTF8

# Читаем строки из CSV
$lines = Get-Content -Path $inputFile
$lineCount = $lines.Count
$currentLine = 0

# Обрабатываем каждую строку
foreach ($line in $lines) {
    $currentLine++

    Write-Host "Обрабатывается: $line"

    # Разделяем по запятым
    $parts = $line -split ','

    if ($parts.Count -ge 3) {
        $fio = $parts[0].Trim()
        $birthdate = $parts[1].Trim()
        $city = $parts[2].Trim()

        # Разделяем ФИО по пробелу
        $fioParts = $fio -split ' '
        if ($fioParts.Count -ge 2) {
            $second_name = $fioParts[0].Trim()
            $first_name = $fioParts[1].Trim()

            # Формируем SQL-строку
            $sqlLine = "('$first_name', '$second_name', '$birthdate', '$city')"

            # Добавляем запятую или точку с запятой в конце строки
            if ($currentLine -lt $lineCount) {
                $sqlLine += ","
            } else {
                $sqlLine += ";"
            }

            # Пишем строку в файл
            $sqlLine | Out-File -FilePath $outputFile -Append -Encoding UTF8
        }
    }
}

