select * from AspNetRoles;
select * from AspNetUsers;
insert into AspNetRoles values('ABCD1234','Admin','Admin','2022-02-01');
insert into AspNetUserRoles values('7e65bfbb-89b2-4816-b3e3-fe0eed3dc15a','ABCD1234');
select * from AspNetUserRoles;