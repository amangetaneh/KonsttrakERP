function GeneralReportGuide() {
  return (
    <div>
      <hr />

      <div>
        <h3>1. Click on Reports</h3>
      </div>

      <div>
        <h3>2. Set rules to filter the data</h3>
        <p>Draw Chart: Defines what value will be used on the Y-axis.</p>
        <p>Include Items: Select what you want to see or compare.</p>
        <p>From Date: Earliest date to be included in the report.</p>
        <p>To Date: Latest date to be included in the report.</p>
        <p>
          Site and Employee: Define which sites and employees will be included.
        </p>
        <p>Item Type: Define the items by type.</p>
        <p>
          Item: Appears when Material is selected for Item Type and a material
          can be selected here.
        </p>
        <p>
          Category and Item: Appear when Equipment is selected for Item Type and
          category and equipment can be selected here.
        </p>
        <p>
          Group By: Defines based on what the transactions will be grouped. For
          example Group By Date can be used to see and compare daily
          transactions.
        </p>
        <p>
          Chart Type: You can select a chart type if you want a line chart
          instead of a bar chart.
        </p>
      </div>

      <div>
        <h3>3. Any number of Included Items can be selected</h3>
      </div>

      <div>
        <h3>4. Click on Generate Report</h3>
        <p>
          When you&apos;re done tuning your selection conditions you can
          generate the reports.
        </p>
      </div>

      <div>
        <h3>5. Click on Reset</h3>
        <p>
          At any point you can click on Reset at any point to clear the report
          generation settings.
        </p>
      </div>
    </div>
  );
}

export default GeneralReportGuide;
