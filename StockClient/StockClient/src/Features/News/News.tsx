import {Box, Checkbox, Divider, Grid, List, ListItem, ListItemText, Paper, TextField, Typography} from "@mui/material";
import { useEffect, useState } from "react";
import axios, { AxiosResponse } from "axios";
import { ConnectionUtility } from "../../App/Utility/ConnectionUtility.tsx";
import NewsList from "./NewsList.tsx";
import { ArticlesGroup } from "../../Models/ArticlesGroup.tsx";
import toast from "react-hot-toast";

export default function News()
{
    const [query, setQuery] = useState<string>();
    const [lookBack, setLookBack] = useState<number>(0);
    const [newsPerSymbol, setNewsParSymbol] = useState<number>(0);
    const [authorsList, setAuthorsList] = useState<string[]>([]);
    const [articles, setArticles] = useState<ArticlesGroup[]>([]);

    useEffect(() => {
        axios.get(`${ConnectionUtility.getServerBaseUrl()}/News/GetNews/symbol=${query}&fromDays=${lookBack}&perSymbol=${newsPerSymbol}`)
        .then(response => manageResponse(response))
        .catch(error => toast.error(error.message, {position: 'top-right'}))
    }, [query, lookBack, newsPerSymbol])

    function manageResponse(response: AxiosResponse)
    {
        if(response?.status >= 200 && response?.status < 300){
            setArticles(response.data);
            setAuthorsList(articles.map(a => a.groupName));
        }
        else{
            toast(response.status.toString())
        }
    }

    function setQueryIf(text: string)
    {
        if(text.length >= 2)
        {
            setQuery(text?.toUpperCase());
        }
    }

    function setLookBackState(lookBack: string)
    {
        let asNumber = parseInt(lookBack);
        setLookBack(asNumber);
    }

    function setNewsPerSymbolState(lookBack: string)
    {
        let asNumber = parseInt(lookBack);
        setNewsParSymbol(asNumber);
    }

    function getAuthors()
    {
        return articles
            .map(group => group.groupName);
    }

    function filterAuthor(author: string)
    {
        if(authorsList.includes(author))
        {
            setAuthorsList(authorsList.filter(a => a !== author));
        }
        else
        {
            setAuthorsList(() => [...authorsList, author])
        }
    }

    function getArticles()
    {
        return articles
            .filter(a => authorsList.includes(a.groupName))
    }

    return(
        <Box sx = {{flexGrow: 1, padding: 2}}>
            <Typography variant="h2" color="text.secondary" sx={{ mb: 3 }}>News</Typography>
            <Box mb={2}>
            <Paper sx={{ padding: 2 }}>
                <TextField
                        onChange={e => setQueryIf(e.target.value)}
                        sx={{ mb: 1.5 }}
                        id="OverviewQuery"
                        fullWidth
                        size="small"
                        label="Search"
                        variant="outlined"/>
            </Paper>
            </Box>
            <Grid container spacing={2}>
                <Grid item xs={12} md={3}>
                    <Paper sx={{ padding: 2 }}>
                        <Typography variant="h6">Parameters</Typography>
                        <Divider sx={{ my: 2 }} />
                        <TextField 
                            onChange={e => setLookBackState(e.target.value == '' ? '0' : e.target.value)}
                            fullWidth
                            id="LookbackDays" 
                            label="Lookback days" 
                            variant="outlined" 
                            size="small"
                            sx={{ mb: 1.5}}
                        />
                        <TextField 
                            onChange={e => setNewsPerSymbolState(e.target.value)}
                            fullWidth
                            id="NewsPerAuthor" 
                            label="News per author" 
                            variant="outlined" 
                            size="small"
                            sx={{ mb: 1.5}} 
                        />
                        <div style={{ padding: '10px', border: '1px solid #ccc', borderRadius: '8px'}}>
                            <h3>Author Filter</h3>
                            <List>
                                {getAuthors().map((author) => (
                                <ListItem key={author} style={{ paddingTop: '0px', paddingBottom: '0px' }}>
                                    <Checkbox
                                    checked={authorsList.includes(author)}
                                    onClick={() => filterAuthor(author)}
                                    tabIndex={-1}
                                    disableRipple
                                    style={{ padding: '4px' }} // Reduce padding inside the checkbox
                                    />
                                    <ListItemText primary={author} style={{ marginLeft: '8px' }} />
                                </ListItem>
                                ))}
                            </List>
                        </div>
                    </Paper>
                </Grid>

                <Grid item xs={12} md={9}>
                    <Paper sx={{padding: 2}}>
                        <Typography variant="h6">Articles</Typography>
                        <Divider sx={{ my: 2}} />
                        <NewsList articlesGroups={getArticles()} />
                    </Paper>
                </Grid>
            </Grid>
        </Box>
    )
}