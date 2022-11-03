# Currency exchange

This currency exchange service keeps track of currency exchange trades done by clients. 


### Features
- HTTP endpoint to request currency exchange trade.
- Validate request.
- Get exchange rates and symbols from Fixer.io
- Cache exchange rate for 30 minutes.
- Limit clients to 10 request per hour.

## How to run

- From a terminal execute 
  - change directory to docker `cd {path}/currency-exchange/docker`
  - `docker compose up db`
- Connect to the database named `exchange` using the following credentials:
  - username: `postgres` 
  - password: `MyPassword!23`  
- Execute the script named `create-exchange-table.sql` from `sql` directory.
- From the docker directory execute `docker compose up` in a terminal.
- From a browser visit [the localhost swagger.](http://localhost:6000/swagger/index.html)