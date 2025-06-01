import http from 'k6/http';
import { check } from 'k6';

export let options = {
  vus: 100,
  duration: '30s',
};

export default function () {
  let res = http.get('http://otus_web_api/user/search?firstName=%D0%98%D0%B2%D0%B0&secondName=%D0%9F%D0%B5%D1%82%D1%80');
  check(res, { 'status is 200': (r) => r.status === 200 });
}