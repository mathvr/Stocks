import React, {useEffect, useState} from "react";
import {StockOverview} from "../../Models/StockOverview.tsx";
import StocksOverview from "../../Features/StocksOverview/StocksOverview.tsx";
import {Container, createTheme, CssBaseline, ThemeProvider, Typography} from "@mui/material";
import Header from "./Header.tsx";
import {Outlet} from "react-router-dom";

function App(){
    const [darkMode, setDarkMode] = useState(false);
    const paletteType = darkMode ? 'dark' : 'light';
    const theme = createTheme({
        palette: {
            primary: {
                light: '#303f9f',
                main: '#303f9f'
            },
            secondary: {
                light: '#9fa8da',
                main: '#9fa8da',
            },
            mode: paletteType,
            background: {
                default: paletteType === 'light' ? '#eaeaea' : '#121212'
            }
        }
    })

    function handleThemeChange()
    {
        setDarkMode(!darkMode);
    }

  return (
    <ThemeProvider theme={theme}>
        <CssBaseline />
        <Header darkMode={darkMode} handleThemeChange={handleThemeChange}/>
        <Container>
            <Outlet />
        </Container>
    </ThemeProvider>
  );
}

export default App;
