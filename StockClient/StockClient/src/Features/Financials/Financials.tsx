import {Box, Button, Divider, Grid, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TextField, Typography} from "@mui/material";
import { useEffect, useState } from "react";
import { Financial } from "../../Models/Financial";
import axios, { AxiosResponse } from "axios";
import toast from "react-hot-toast";
import { ConnectionUtility } from "../../App/Utility/ConnectionUtility";
import StockOverviewList from "../StocksOverview/StockOverviewList";

export default function Financials()
{
    const [financial, setFinancial] = useState<Financial[]>([]);
    const [query, setQuery] = useState<string>("");

    useEffect(() => {
        setFinancial([]);
        if(query?.length != null && query.length > 0)
        {
            axios.get(`${ConnectionUtility.getServerBaseUrl()}/Financials/GetBySymbol/${query}`)
            .then(response => manageResponse(response))
            .catch(error => toast.error(error.message, {position: 'top-right'}))
        }
    }, [query])

    function manageResponse(response: AxiosResponse)
    {
        if(response?.status >= 200 && response?.status < 300){
            setFinancial(response.data);
        }
        else{
            toast(response.status.toString())
        } 
    }


    return(
      <>
        <Box sx={{ flexGrow: 1, padding: 2 }}>
          <Typography variant="h2" color="text.secondary" sx={{ mb: 3 }}>
            Financials
          </Typography>
          <Grid container spacing={3}>
            <Grid item xs={12} md={9}>
              <Paper sx={{ padding: 3, borderRadius: 2, boxShadow: 3 }}>
                {/* Search Bar */}
                <TextField
                  onChange={(e) => setQuery(e.target.value)}
                  sx={{ mb: 3 }}
                  id="OverviewQuery"
                  fullWidth
                  size="small"
                  label="Search Financial Data"
                  variant="outlined"
                />
                {financial?.map((f, index) => (
                  <Box key={index} sx={{ mb: 4 }}>
                    {/* Financial Metadata */}
                    <Paper
                      elevation={2}
                      sx={{
                        mb: 2,
                        padding: 2,
                        borderRadius: 2,
                        backgroundColor: '#444444',
                      }}
                    >
                      <Typography variant="h6" sx={{ mb: 1, color: '#ffffff' }}>
                        Year: <strong>{f.year}</strong>
                      </Typography>
                      <Typography variant="body1" color="text.secondary" sx={{ color: '#cccccc' }}>
                        From: <strong>{f.fromDate}</strong> | To: <strong>{f.toDate}</strong> | Symbol: <strong>{f.symbol}</strong>
                      </Typography>
                    </Paper>

                    {/* Financial Properties Table */}
                    <TableContainer component={Paper} elevation={3} sx={{ borderRadius: 2 }}>
                      <Table size="small">
                        <TableHead>
                          <TableRow sx={{ backgroundColor: '#333333' }}>
                            <TableCell
                              align="center"
                              sx={{ fontSize: '1rem', color: '#ffffff', fontWeight: 'bold' }}
                            >
                              Property
                            </TableCell>
                            <TableCell
                              align="center"
                              sx={{ fontSize: '1rem', color: '#ffffff', fontWeight: 'bold' }}
                            >
                              Value
                            </TableCell>
                            <TableCell
                              align="center"
                              sx={{ fontSize: '1rem', color: '#ffffff', fontWeight: 'bold' }}
                            >
                              Unit
                            </TableCell>
                          </TableRow>
                        </TableHead>
                        <TableBody>
                          {f?.properties.map((prop, idx) => (
                            <TableRow
                              key={idx}
                              sx={{
                                backgroundColor: "#FAFAFA",
                                '&:hover': { backgroundColor: '#EFE5C9' },
                              }}
                            >
                              <TableCell
                                component="th"
                                scope="row"
                                align="center"
                                sx={{ fontSize: '0.95rem', fontWeight: 'medium', color: '#555555' }}
                              >
                                {prop.name}
                              </TableCell>
                              <TableCell
                                align="center"
                                sx={{ fontSize: '0.95rem', color: '#444444' }}
                              >
                                {prop.value}
                              </TableCell>
                              <TableCell
                                align="center"
                                sx={{ fontSize: '0.95rem', color: '#444444' }}
                              >
                                {prop.unit}
                              </TableCell>
                            </TableRow>
                          ))}
                        </TableBody>
                      </Table>
                    </TableContainer>
                  </Box>
                ))}
              </Paper>
            </Grid>
          </Grid>
        </Box>
      </> 
    )
}