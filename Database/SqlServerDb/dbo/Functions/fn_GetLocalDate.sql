CREATE FUNCTION dbo.fn_GetLocalDate()
RETURNS datetime2
AS
BEGIN
	DECLARE @Date DATETIME = GETDATE()
	--DECLARE @varchar VARCHAR(max) = 'Microsoft SQL Azure (RTM) - 11.0.9228.18   Dec  1 2014 19:44:12   Copyright (c) Microsoft Corporation'
	--if(@varchar like '%SQL Azure%')
	if(@@VERSION like '%SQL Azure%')	
	begin
		SET @Date = DATEADD (hh, -5, @Date)
	end
		
	RETURN @Date
END