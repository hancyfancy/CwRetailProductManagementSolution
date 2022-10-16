-- drop stored procedures
DROP TRIGGER IF EXISTS production.products_tr_delete
DROP TRIGGER IF EXISTS production.products_tr_insert
DROP TRIGGER IF EXISTS production.products_tr_update

-- drop tables
DROP TABLE IF EXISTS audit.products

-- drop schemas
DROP SCHEMA IF EXISTS audit
