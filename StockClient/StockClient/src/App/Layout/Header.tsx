import {AppBar, Box, FormControlLabel, IconButton, List, ListItem, Switch, Toolbar, Typography} from "@mui/material";
import {NavLink} from "react-router-dom";
import DownloadingIcon from '@mui/icons-material/Downloading';
import {useState} from "react";
import axios from "axios";
import { ConnectionUtility } from "../Utility/ConnectionUtility";

const midLinks = [
    {title: 'Overview', path: '/overview'},
    {title: 'About', path: '/about'},
    {title: 'Performance', path: '/performance'},
    {title: 'News', path: '/news'},
    {title: 'Financials', path: '/financials'}
]

const rightLinks = [
    {title: 'Login', path: '/login'},
]

const navStyles = {
    color: 'inherit',
    typography:"h6",
    textDecoration: 'none',
    '&:hover': {
        color: 'secondary.main'
    },
    '&.active': {
        color: 'secondary.main'
    }
}

interface Props{
    darkMode: boolean;
    handleThemeChange : () => void;
}
export default function Header({darkMode, handleThemeChange}: Props){

    const [downloadSeriesResponse, setDownloadSeriesResponse] = useState<number>();
    const [downloadReputationResponse, setDownloadReputationResponse] = useState<number>()
    const [downloadNewsResponse, setDownloadNewsResponse] = useState<number>()

    const daysAgoNews = 30;


    function delay(ms: number) {
        return new Promise( resolve => setTimeout(resolve, ms) );
    }

    async function downloadTimeSeries()
    {
        axios.post(`${ConnectionUtility.getServerBaseUrl()}/TimeSeries/DownloadValues/FromDaysAgo=360`)
            .then(async response =>
            {
                setDownloadSeriesResponse(response.status)
                await delay(2000)
                setDownloadSeriesResponse(null)
            })
            .catch(async error => {
                setDownloadSeriesResponse(error.response.status)
                await delay(2000)
                setDownloadSeriesResponse(null)
            })
    }

    async function downloadReputations()
    {
        axios.get(`${ConnectionUtility.getServerBaseUrl()}/StocksOverview/Reputation/DownloadValues`)
        .then(async response =>
            {
                setDownloadReputationResponse(response.status)
                await delay(2000)
                setDownloadReputationResponse(null)
            })
            .catch(async error => {
                setDownloadReputationResponse(error.response.status)
                await delay(2000)
                setDownloadReputationResponse(null)
            })
    }

    async function downloadNews()
    {
        axios.post(`${ConnectionUtility.getServerBaseUrl()}/News/DownloadNews/fromDaysAgo=${daysAgoNews.toString()}`)
        .then(async response =>
            {
                setDownloadNewsResponse(response.status)
                await delay(2000)
                setDownloadNewsResponse(null)
            })
            .catch(async error => {
                setDownloadNewsResponse(error.response.status)
                await delay(2000)
                setDownloadNewsResponse(null)
            })
    }

    return(
        <AppBar position ='static' sx={{mb: 4}}>
            <Toolbar sx = {{display: 'flex', justifyContent: 'space-between', alignItems: 'center'}}>
                <Box display='flex' alignItems='center'>
                    <Typography variant='h4' component={NavLink}
                                to='/'
                                sx={navStyles}>
                        HomeStocks
                    </Typography>
                    <FormControlLabel
                        sx={{ml: 1}}
                        control={<Switch checked={darkMode} onChange={handleThemeChange}/>}
                        label={darkMode ? 'Light Mode' : 'Dark Mode'}
                    />
                    <IconButton
                        size="small"
                        color="secondary"
                        onClick={downloadTimeSeries}>
                        <DownloadingIcon/>
                        <Typography
                            sx ={{mt: 2}}
                            variant="body2"
                            color= {downloadSeriesResponse == 200 ? '#28a745' : '#ff0000'}
                        >{downloadSeriesResponse == 200 ? 'Success' : ''}</Typography>
                    </IconButton>
                    <IconButton
                        size="small"
                        color="secondary"
                        onClick={downloadReputations}>
                        <DownloadingIcon/>
                        <Typography
                            sx ={{mt: 2}}
                            variant="body2"
                            color= {downloadReputationResponse == 200 ? '#28a745' : '#ff0000'}
                        >{downloadReputationResponse == 200 ? 'Success' : ''}</Typography>
                    </IconButton>
                    <IconButton
                        size="small"
                        color="secondary"
                        onClick={downloadNews}>
                        <DownloadingIcon/>
                        <Typography
                            sx ={{mt: 2}}
                            variant="body2"
                            color= {downloadNewsResponse == 200 ? '#28a745' : '#ff0000'}
                        >{downloadNewsResponse == 200 ? 'Success' : ''}</Typography>
                    </IconButton>
                </Box>
                <Box display='flex' alignItems='center'>
                    <List sx={{display: 'flex'}}>
                        {midLinks.map(({title, path}) =>
                            (
                                <ListItem
                                    component={NavLink}
                                    to={path}
                                    key={path}
                                    sx={navStyles}
                                >
                                    {title}
                                </ListItem>
                            ))}
                    </List>
                </Box>
                <Box display='flex' alignItems='center'>
                    <List sx={{display: 'flex'}}>
                        {rightLinks.map(({title, path}) =>
                            (
                                <ListItem
                                    component={NavLink}
                                    to={path}
                                    key={path}
                                    sx={navStyles}>
                                    {title}
                                </ListItem>
                            ))}
                    </List>
                </Box>
            </Toolbar>
        </AppBar>
    )
}