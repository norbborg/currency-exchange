CREATE TABLE IF NOT EXISTS public.exchange (
    id serial primary KEY,
    clientId INTEGER not null,
    fromCurrency VARCHAR(3) not null,
    toCurrency VARCHAR(3) not null,
    exchangeRate DECIMAL not null,
    amount DECIMAL not null,
    datecreated timestamp default current_timestamp not null
);