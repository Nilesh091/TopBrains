CREATE FUNCTION Sales.fn_SalesByStore (@storeid INT)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        P.ProductID,
        P.Name,
        SUM(SD.LineTotal) AS 'YTD Total'
    FROM Production.Product AS P
    JOIN Sales.SalesOrderDetail AS SD 
        ON SD.ProductID = P.ProductID
    JOIN Sales.SalesOrderHeader AS SH 
        ON SH.SalesOrderID = SD.SalesOrderID
    WHERE SH.CustomerID = @storeid
    GROUP BY P.ProductID, P.Name
);
GO

SELECT * 
FROM Sales.fn_SalesByStore(602);
