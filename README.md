Необходимо разработать веб-сервис с одним  http-методом, который будет:
1. Принимать на вход url ссылку, на какой-нибудь веб-ресурс, например 
```
https://ya.ru/
```
2. Отправлять на этот адрес GET запрос, сохранять status code ответа
    1. Если в кэше есть картинка по этому статус коду, то пропустить пункты 3, 5
3. По api получать картинку со статус кодом с сервиса 
```
https://http.cat/
```
4. Вернуть в ответе на запрос картинку с котиком 
5. Кешировать на определенное время эту картинку в другом потоке
6. Обработать возможные ошибки
