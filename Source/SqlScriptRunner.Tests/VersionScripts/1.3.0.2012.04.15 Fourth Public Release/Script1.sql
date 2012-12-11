CREATE TABLE t(x INTEGER PRIMARY KEY, y integer, z integer);
GO
Insert into T (y, x) values(1,13);
GO
Insert into T (y, x) values(2,15);
GO
Insert into T (y, x) values(3,17);
GO
Insert into T (y, x) values(4,12);
Insert into T (y, x) values(5,1324);
Insert into T (y, x) values(6,166);
Insert into T (y, x) values(7,13625);
Insert into T (y, x) values(8,124525);
GO
select * from t;
GO
drop table t;
