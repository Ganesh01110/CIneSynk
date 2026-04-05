import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '30s', target: 50 }, // Ramp up to 50 users
        { duration: '1m', target: 50 },  // Stay at 50 users
        { duration: '30s', target: 100 }, // Spike to 100 users
        { duration: '30s', target: 0 },   // Ramp down
    ],
    thresholds: {
        http_req_duration: ['p(95)<500'], // 95% of requests must be under 500ms
        http_req_failed: ['rate<0.01'],    // less than 1% errors
    },
};

const BASE_URL = __ENV.API_URL || 'http://localhost:5000/api';

export default function () {
    // 1. Get Shows
    const showsRes = http.get(`${BASE_URL}/shows`);
    check(showsRes, {
        'get shows status was 200': (r) => r.status === 200,
    });

    sleep(1);

    // 2. Simulate a Reservation attempt (Requires Auth usually, but we check for failure too)
    // Note: In a real test, we would have a pool of user logins
    const payload = JSON.stringify({
        showId: 1,
        seatId: Math.floor(Math.random() * 100) + 1,
        userId: 1,
    });

    const params = {
        headers: {
            'Content-Type': 'application/json',
            'X-Idempotency-Key': `load-test-${__VU}-${__ITER}`,
        },
    };

    const reserveRes = http.post(`${BASE_URL}/bookings/reserve`, payload, params);
    check(reserveRes, {
        'reserve seat status was 200 or 400': (r) => r.status === 200 || r.status === 400,
    });

    sleep(1);
}
