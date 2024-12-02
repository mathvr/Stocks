import {Box, Checkbox, Divider, Grid, List, ListItem, ListItemText, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TextField, Typography} from "@mui/material";
import { useEffect, useState } from "react";
import { PerformanceApiModel } from "../../Models/PerformanceApiModel";
import { ConnectionUtility } from "../../App/Utility/ConnectionUtility";
import axios, { AxiosResponse } from "axios";
import toast from "react-hot-toast";

export default function Performance()
{
    const [performanceData, setPerformanceData] = useState<PerformanceApiModel>()
    const [sinceDays, setSinceDays] = useState<number>(1)

    useEffect(() => {
        axios.get(`${ConnectionUtility.getServerBaseUrl()}/Computation/PriceEvolution/sinceDays=${sinceDays}`)
        .then(response => setPerformanceData(response.data))
        .catch(error => toast.error(error.message, {position: 'top-right'}))
    }, [sinceDays])

    function setSinceDaysState(sinceDays: string)
    {
        try
        {
            let asNumber = parseInt(sinceDays);

            if(typeof(asNumber) === "number")
            {
                setSinceDays(asNumber);
            }
            else
            {
                setSinceDays(0);
            }
        }
        catch(error)
        {
            setSinceDays(0);
        }
    }

    function getColor(percentage: number)
    {
        if(percentage > 0){
            return "#B3FFB3";
        }

        if(percentage == 0){
            return "#FFFFB3";
        }

        if(percentage < 0){
            return "#FFB3B3";
        }
    }

        return(
            <Box sx = {{flexGrow: 1, padding: 0}}>
                <Typography variant="h2" color="text.secondary" sx={{ mb: 3 }}>Stocks Performance</Typography>
                <Grid container spacing={2}>
                    <Grid item xs={12} md={3}>
                        <Paper sx={{ padding: 2 }}>
                            <Typography variant="h6">Parameters</Typography>
                            <Divider sx={{ my: 2 }} />
                            <TextField 
                                onChange={e => setSinceDaysState(e.target.value)}
                                fullWidth
                                id="sinceDays" 
                                label="Since Days" 
                                variant="outlined" 
                                size="small"
                                sx={{ mb: 1.5}}
                        />
                            </Paper>
                    </Grid>
                    <Grid item xs={12} md={9}>
                        <Paper sx={{ padding: 2 }}>
                            <Typography variant="h6">Data</Typography>
                            <Divider sx={{ my: 2 }} />
                            <TableContainer>
                                <Table size="small">
                                    <TableHead>
                                        <TableRow sx={{backgroundColor: "#333333"}}>
                                            <TableCell align="center" sx={{ fontSize: '0.9rem', padding: '6px', color: "white" }}>Title (Symbol)</TableCell>
                                            <TableCell align="center" sx={{ fontSize: '0.9rem', padding: '6px', color: "white" }}>Value</TableCell>
                                            <TableCell align="center" sx={{ fontSize: '0.9rem', padding: '6px', color: "white" }}>In Date</TableCell>
                                            <TableCell align="center" sx={{ fontSize: '0.9rem', padding: '6px', color: "white" }}>Price Difference</TableCell>
                                            <TableCell align="center" sx={{ fontSize: '0.9rem', padding: '6px', color: "white" }}>Price Difference(%)</TableCell>
                                            <TableCell align="center" sx={{ fontSize: '0.9rem', padding: '6px', color: "white" }}>Split Occured</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {performanceData?.models.map((model) => (
                                            <TableRow key={model.name} sx={{'&:last-child td, &:last-child th': { border: 0 }, backgroundColor: getColor(model.percent)}}>
                                                <TableCell component="th" scope="row" align="center" sx={{ fontSize: '0.85rem', padding: '6px', backgroundColor: "#333333", color: "white" }}>{model.name}</TableCell>
                                                <TableCell align="center" sx={{ fontSize: '0.85rem', padding: '6px', color: "#333333" }}>{model.currentValue}</TableCell>
                                                <TableCell align="center" sx={{ fontSize: '0.85rem', padding: '6px', color: "#333333" }}>{model.valueDate}</TableCell>
                                                <TableCell align="center" sx={{ fontSize: '0.85rem', padding: '6px', color: "#333333" }}>{model.difference}</TableCell>
                                                <TableCell align="center" sx={{ fontSize: '0.85rem', padding: '6px', color: "#333333" }}>{model.percent}%</TableCell>
                                                <TableCell align="center" sx={{ fontSize: '0.85rem', padding: '6px', color: "#333333" }}>{model.hasSplit ? "Yes" : "No"}</TableCell>
                                            </TableRow>
                                        ))}
                                    </TableBody>
                                </Table>
                            </TableContainer>
                        </Paper>
                    </Grid>
                </Grid>
            </Box>
        )
}