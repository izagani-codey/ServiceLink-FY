SELECT u.email, r.Name
FROM AspNetUsers u
JOIN AspNetUserRoles ur  ON u.Id = ur.UserId
JOIN AspNetRoles r ON r.Id = Ur.RoleId