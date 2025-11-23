# JWT Authentication API

## Огляд

API використовує JSON Web Token (JWT) для автентифікації та авторизації користувачів.

## Endpoints

### 1. Автентифікація (Login)

**POST** `/api/auth/login`

**Request Body:**
```json
{
  "email": "admin@charityhub.com",
  "password": "Admin123!"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "admin@charityhub.com",
  "firstName": "Admin",
  "lastName": "User"
}
```

**Response (401 Unauthorized):**
```json
{
  "message": "Invalid email or password"
}
```

### 2. Захищені Endpoints

Всі endpoints в `/api/events` вимагають авторизації.

**Приклад використання токену:**

Додайте заголовок `Authorization` до запиту:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**GET** `/api/events` - Отримати всі події (потрібна авторизація)

**GET** `/api/events/{id}` - Отримати подію за ID (потрібна авторизація)

**POST** `/api/events` - Створити подію (потрібна авторизація)

**PUT** `/api/events/{id}` - Оновити подію (потрібна авторизація)

**DELETE** `/api/events/{id}` - Видалити подію (потрібна авторизація + роль Admin)

## Приклади використання

### cURL

1. **Логін:**
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@charityhub.com","password":"Admin123!"}'
```

2. **Отримати події (з токеном):**
```bash
curl -X GET https://localhost:5001/api/events \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### JavaScript (Fetch API)

```javascript
// 1. Логін
const loginResponse = await fetch('https://localhost:5001/api/auth/login', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json'
  },
  body: JSON.stringify({
    email: 'admin@charityhub.com',
    password: 'Admin123!'
  })
});

const loginData = await loginResponse.json();
const token = loginData.token;

// 2. Використання токену для захищених запитів
const eventsResponse = await fetch('https://localhost:5001/api/events', {
  method: 'GET',
  headers: {
    'Authorization': `Bearer ${token}`
  }
});

const events = await eventsResponse.json();
```

### Postman

1. Створіть запит POST на `/api/auth/login`
2. В тілі запиту (Body -> raw -> JSON) додайте:
```json
{
  "email": "admin@charityhub.com",
  "password": "Admin123!"
}
```
3. Скопіюйте токен з відповіді
4. Для захищених запитів:
   - Перейдіть на вкладку "Authorization"
   - Виберіть тип "Bearer Token"
   - Вставте скопійований токен

## Swagger UI

Після запуску API, відкрийте Swagger UI за адресою:
- `https://localhost:5001/swagger` (HTTPS)
- `http://localhost:5000/swagger` (HTTP)

У Swagger UI:
1. Натисніть кнопку "Authorize" (🔒)
2. Введіть токен у форматі: `Bearer YOUR_TOKEN_HERE`
3. Тепер всі захищені endpoints будуть доступні

## Налаштування

JWT налаштування знаходяться в `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!",
    "Issuer": "CharityHubAPI",
    "Audience": "CharityHubAPI",
    "ExpirationInMinutes": 60
  }
}
```

**Важливо:** Для продакшену змініть `SecretKey` на безпечний випадковий ключ!

## Тестові облікові дані

При першому запуску створюється адміністратор:
- **Email:** `admin@charityhub.com`
- **Password:** `Admin123!`
- **Role:** Admin

