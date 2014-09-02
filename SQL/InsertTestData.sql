USE TestDB
GO

DELETE myschema.Name
GO

INSERT INTO myschema.Name (FirstName,LastName,Age) 
                   VALUES ('Libby',  'Watson',24)
INSERT INTO myschema.Name (FirstName,LastName,Age) 
                   VALUES ('Chelsea','Gibbons',45)
INSERT INTO myschema.Name (FirstName,LastName,Age) 
                   VALUES ('Alfie',  'Stevens',32)
INSERT INTO myschema.Name (FirstName,LastName,Age) 
                   VALUES ('Holly',  'Webb',  43)
INSERT INTO myschema.Name (FirstName,LastName,Age) 
                   VALUES ('Chelsea','Booth', 55)
INSERT INTO myschema.Name (FirstName,LastName,Age) 
                   VALUES ('Logan',  'Barrett',29)
INSERT INTO myschema.Name (FirstName,LastName,Age) 
                   VALUES ('Aidan',  'Bell',  87)
INSERT INTO myschema.Name (FirstName,LastName,Age) 
                   VALUES ('William','Morgan',73)
INSERT INTO myschema.Name (FirstName,LastName,Age) 
                   VALUES ('Evan',  'Rowe',   37)
GO
