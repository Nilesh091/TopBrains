create function HumanResources.unfEmployeeNames(@format nvarchar(9))
returns @tbl_employees table 
(EmploreeId int PRIMARY KEY,[Employee name] nvarchar(100))
AS
BEGIN
if(@format='SHOETNAME')
INSERT @tbl_employees
SELECT EmployeeId,lastname
from HumanResources.vEmployee
else if(@format='LONGNAME')
INSERT @tbl_employees
SELECT EmployeeId,(FirstName+' '+LastName)
from HumanResources.vEmployee
RETURN
END;

SELECT * from HumanResources.unfEmployeeNames('LONGNAME')

SELECT * from HumanResources.unfEmployeeNames('SHOETNAME')