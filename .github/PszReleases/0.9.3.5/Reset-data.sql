
use Budget
go

-- Reset Budget data

delete from dbo.Article_Order;
delete from dbo.Budget_Order;
delete from dbo.Project_Budget;
delete from dbo.Validators_Project;
delete from dbo.__FNC_OrderRejection;
delete from dbo.__FNC_OrderValidation;
delete from dbo.budget_order_version;
delete from dbo.Budget_Article_Version;

-- reset total spent
update dbo.Budget_users set TotalSpent=0;
update dbo.assign_budget_departement set TotalSpent=0;
update dbo.assign_budget_land set TotalSpent=0;


-- Allocations
delete from dbo.Budget_users;
delete from dbo.assign_budget_departement;
delete from dbo.assign_budget_land;
delete from dbo.Supplement_Budget_Land;


-- Config
delete from [dbo].[Land_Department_Joint];

delete from dbo.departement_user_joint;
delete from dbo.Budget_departement;
delete from dbo.Budget_lands;


-- Administration