-- up
CREATE TABLE IF NOT EXISTS products (
    Id SERIAL PRIMARY KEY NOT NULL,
    name TEXT NOT NULL,
    price NUMERIC NOT NULL,
    discount NUMERIC NULL,
    in_stock BOOLEAN NOT NULL
);


-- down
DROP TABLE IF EXISTS products;

