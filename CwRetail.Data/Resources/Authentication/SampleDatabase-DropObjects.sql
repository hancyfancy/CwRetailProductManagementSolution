-- drop tables
DROP TABLE IF EXISTS auth.roles
DROP TABLE IF EXISTS auth.userroles
DROP TABLE IF EXISTS auth.userverification
DROP TABLE IF EXISTS auth.users

-- drop stored procedures
DROP FUNCTION IF EXISTS auth.users_checkvalidusername
DROP FUNCTION IF EXISTS auth.users_checkvalidemail

-- drop schemas
DROP SCHEMA IF EXISTS auth
