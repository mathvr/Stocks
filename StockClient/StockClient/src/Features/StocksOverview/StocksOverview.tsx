import React, {Fragment, useEffect, useState} from "react";
import {StockOverview} from "../../Models/StockOverview.tsx";
import {Box, Button, Divider, Grid, List, ListItem, Paper, TextField, Typography} from "@mui/material";
import StockOverviewList from "./StockOverviewList.tsx";
import axios, { AxiosResponse } from "axios";
import { ConnectionUtility } from "../../App/Utility/ConnectionUtility.tsx"
import toast, { Toaster } from "react-hot-toast";
import DownloadIcon from '@mui/icons-material/Download';

export default function StocksOverview() {
    const [overviews, setOverviews] = useState<StockOverview[]>([]);
    const [query, setQuery] = useState<string>();
    const [addTitle, setAddTitle] = useState<string>();

    const top = 20;

    useEffect(() => {
        setOverviews([]);
        if(query?.length != null && query.length > 0)
        {
            axios.get(`${ConnectionUtility.getServerBaseUrl()}/StocksOverview/GetCompanyOverviews/query=${query}&top=${top}`)
            .then(response => manageResponse(response))
            .catch(error => toast.error(error.message, {position: 'top-right'}))
        }
    }, [query])

    const downloadTitle = () => {
        axios.post(`${ConnectionUtility.getServerBaseUrl()}/StocksOverview/AddOverview/symbol=${addTitle}`)
        .then(response => managePostResponse(response))
        .catch(error => toast.error(error.response?.data, {position: 'top-right'}))
    }

    function setQueryIf(text: string)
    {
        setQuery(text)
    }

    function manageResponse(response: AxiosResponse)
    {
        if(response?.status >= 200 && response?.status < 300){
            setOverviews(response.data);
        }
        else{
            toast(response.status.toString())
        }
    }

    const managePostResponse = (response: AxiosResponse) => {
        if(response?.status >= 200 && response?.status < 300){
            toast.success(response?.data,{position: 'top-right'})
        }
        else{
            toast.error(response?.data, {position: 'top-right'})
        }
    }

    return(
        <>
à            <Box sx = {{flexGrow: 1, padding: 2}}>
            <Typography variant="h2" color="text.secondary" sx={{ mb: 3 }}>Available Titles</Typography>
                <Grid container spacing={2}>
                <Grid item xs={12} md={3}>
                    <Paper sx={{ padding: 2 }}>
                    <Typography variant="h6">Add Title</Typography>
                    <Divider sx={{ my: 2 }} />
                    <Grid container spacing={1} alignItems="flex-end">
                        <Grid item xs>
                            <TextField 
                                onChange={e => setAddTitle(e.target.value)}
                                fullWidth
                                id="AddTitle" 
                                label="Title Symbol" 
                                variant="outlined" 
                                size="small"
                                sx={{ mb: 1.5 }}
                            />
                        </Grid>
                        <Grid item>
                            <Button
                                variant="contained"
                                color="secondary"
                                size="medium"
                                sx={{ minWidth: 'auto', paddingX: 1.5, mb: 1.5 }}
                                onClick={() => downloadTitle()}
                            >
                            <DownloadIcon fontSize="small" />
                            </Button>
                        </Grid>
                    </Grid>
                    </Paper>
                </Grid>
                <Grid item xs={12} md={9}>
                    <Paper sx={{ padding: 2 }}>
                        <TextField
                            onChange={e => setQueryIf(e.target.value)}
                            sx={{ mb: 1.5 }}
                            id="OverviewQuery"
                            fullWidth
                            size="small"
                            label="Search"
                            variant="outlined"/>
                        <StockOverviewList overviews={overviews ?? null} />
                    </Paper>
                </Grid>
            </Grid>
            </Box>
        </>
    )
}