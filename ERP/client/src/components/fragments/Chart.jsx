import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend,
    BarElement,
    ArcElement,
} from "chart.js";
import { Bar, Line, Pie } from "react-chartjs-2";

ChartJS.register(
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    BarElement,
    ArcElement
);

function Chart({ type, data }) {
    data.datasets[0] = {
        ...data.datasets[0],
        backgroundColor: [
            "rgba(255, 99, 132, 0.6)",
            "rgba(54, 162, 235, 0.6)",
            "rgba(255, 206, 86, 0.6)",
            "rgba(75, 192, 192, 0.6)",
            "rgba(153, 102, 255, 0.6)",
            "rgba(255, 159, 64, 0.6)",
        ],

        borderColor: [
            "rgba(255, 99, 132, 1)",
            "rgba(54, 162, 235, 1)",
            "rgba(255, 206, 86, 1)",
            "rgba(75, 192, 192, 1)",
            "rgba(153, 102, 255, 1)",
            "rgba(255, 159, 64, 1)",
        ],
        borderWidth: 1,
    };

    const options = {
        scales: {
            y: {
                beginAtZero: true,
            },
        },
    };

    if (type == "line")
        return (
            <div className="mb-4" style={{ maxHeight: "50%" }}>
                <Line data={data} options={options} />
            </div>
        );

    return (
        <div className="mb-4" style={{ maxHeight: "50%" }}>
            <Bar data={data} options={options} />
        </div>
    );
}

export default Chart;
