-- drop triggers
DROP TRIGGER IF EXISTS auth.users_tr_delete
DROP TRIGGER IF EXISTS auth.roles_tr_delete

-- drop tables
DROP TABLE IF EXISTS auth.usertokens
DROP TABLE IF EXISTS auth.userroles
DROP TABLE IF EXISTS auth.userverification
DROP TABLE IF EXISTS auth.users
DROP TABLE IF EXISTS auth.roles

-- drop stored procedures
DROP FUNCTION IF EXISTS auth.users_checkvalidusername
DROP FUNCTION IF EXISTS auth.users_checkvalidemail

-- drop schemas
DROP SCHEMA IF EXISTS auth
