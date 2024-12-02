import {createBrowserRouter} from "react-router-dom";
import App from "../Layout/App.tsx";
import HomePage from "../../Features/Home/HomePage.tsx";
import StocksOverview from "../../Features/StocksOverview/StocksOverview.tsx";
import About from "../../Features/About/About.tsx";
import Performance from "../../Features/Stock Performance/Performance.tsx";
import Login from "../../Features/Login/Login.tsx";
import News from "../../Features/News/News.tsx";
import Financials from "../../Features/Financials/Financials.tsx";

export const router = createBrowserRouter([
    {
        path: '/',
        element: <App />,
        children: [
            {path: '', element: <HomePage/>},
            {path: 'Overview', element: <StocksOverview/>},
            {path: 'About', element: <About/>},
            {path: 'Performance', element: <Performance/>},
            {path: 'Login', element: <Login/>},
            {path: 'News', element: <News/>},
            {path: 'Financials', element: <Financials/>}
        ]
    }
])