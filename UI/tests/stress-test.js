import http from 'k6/http';

export const options = {
  vus: 100,
  duration: '30s',
};

const BASE = 'http://localhost:5099/api';

export default function () {

  http.get(`${BASE}/users`);

  http.get(`${BASE}/docs`);

  http.get(`${BASE}/groups/exportarPdf/1`);

  http.post(`${BASE}/auth/login`, JSON.stringify({
    email: "test@test.com",
    password: "123456"
  }), {
    headers: { 'Content-Type': 'application/json' }
  });

}