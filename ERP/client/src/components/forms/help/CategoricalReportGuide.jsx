function CategoricalReportGuide() {
  return (
    <div>
      <hr />

      <div>
        <h3>1. Click on Reports</h3>
      </div>

      <div>
        <h3>
          2. Click on the type of transaction you want a report to be generated
        </h3>
        <p>
          The default is the general report but In this case Issue is selected.
        </p>
      </div>

      <div>
        <h3>3. Selected the Date you want to limit</h3>
        <p>
          For example, if Requested is selected the From Date and To Date will
          limit the Requested Date of the Issue.
        </p>
      </div>

      <div>
        <h3>4. Selecting Group By</h3>
        <p>
          Group By defines how the Issues will be grouped, the options include
          Status, Dates, Weeks, Months, Year and Employees.
        </p>
      </div>

      <div>
        <h3>5. Click on Generate Report</h3>
        <p>
          All other settings are similar to general reports and Reset can be
          used to Once the setting are what you want then you can generate the
          report.
        </p>
      </div>
    </div>
  );
}

export default CategoricalReportGuide;
