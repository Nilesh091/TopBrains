-- create table Employee(
-- 	Id int primary key,
-- 	Name VARCHAR(45),
-- 	Salary int,
-- 	Gender Varchar(12),
-- 	DepartmentId int
-- )


-- insert into Employee Values (1,'Steffan',82000,'Male',3),
-- (2,'Amelia',52000,'Female',2),
-- (3,'Antonio',25000,'male',1),
-- (4,'Marco',47000,'Male',2),
-- (5,'Eliana',46000,'Female',3)

-- CREATE TABLE Employee_Audit
-- (
--     AuditId INT IDENTITY PRIMARY KEY,
--     EmpId INT,
--     ActionType VARCHAR(20),
--     ActionDate DATETIME
-- );

-- CREATE TRIGGER trg_AfterInsert_Employee
-- ON Employee
-- AFTER INSERT
-- AS
-- BEGIN
--     INSERT INTO Employee_Audit (EmpId, ActionType, ActionDate)
--     SELECT Id, 'INSERT', GETDATE()
--     FROM inserted;
-- END;

-- CREATE TRIGGER trg_AfterUpdate_Employee
-- ON Employee
-- AFTER UPDATE
-- AS
-- BEGIN
--     IF UPDATE(Salary)
--     BEGIN
--         INSERT INTO Employee_Audit (EmpId, ActionType, ActionDate)
--         SELECT Id, 'SALARY UPDATED', GETDATE()
--         FROM inserted;
--     END
-- END;

-- CREATE TRIGGER trg_AfterDelete_Employee
-- ON Employee
-- AFTER DELETE
-- AS
-- BEGIN
--     INSERT INTO Employee_Audit (EmpId, ActionType, ActionDate)
--     SELECT Id, 'DELETE', GETDATE()
--     FROM deleted;
-- END;

insert into Employee Values (6,'Nilu',89000,'Male',3);

