CREATE FUNCTION dbo.ufnGetStock1 (@ProductID INT)
RETURNS INT
AS
BEGIN
    DECLARE @ret INT;

    SELECT @ret = SUM(Quantity)
    FROM Production.ProductInventory
    WHERE ProductID = @ProductID
      AND LocationID = 6;

    IF (@ret IS NULL)
        SET @ret = 0;

    RETURN @ret;
END;
GO

SELECT ProductID,
       Name,
       dbo.ufnGetStock1(ProductID) AS CurrentSupply
FROM Production.Product;