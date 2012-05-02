If you wish to update the reference to the WSDL, you'll have to manually open the Reference.cs file created in
{SolutionFolder}\Omniture\Service References\Api13\ and remove the dots (".") from all the method names:
- CodeManager
- Company
- Dashboard
- DataSource
- DataWarehouse
- DeliveryList
- Discover
- Logs
- Permissions
- Report
- ReportSuite
- Saint
- Scheduling
- Survey
- User

For each namespace above do search & replace on "namespace. replace by "namespace
Example:
- Search for "CodeManager.
- Replace by "CodeManager