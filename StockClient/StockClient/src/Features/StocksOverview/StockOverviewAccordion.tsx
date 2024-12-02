import {
    Accordion, AccordionActions,
    AccordionDetails, AccordionSummary,
    Button,
    Card,
    CardActions,
    CardContent,
    List,
    ListItem,
    ListItemIcon,
    ListItemText,
    Typography
} from "@mui/material";
import React, {Fragment, useState} from "react";
import {StockOverview} from "../../Models/StockOverview.tsx";
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import axios from "axios";
import { ConnectionUtility } from "../../App/Utility/ConnectionUtility.tsx";
import InfoOutlinedIcon from '@mui/icons-material/InfoOutlined';
import StarRateIcon from '@mui/icons-material/StarRate';
import toast from "react-hot-toast";

interface Props{
    overview : StockOverview;
}

export default function StockOverviewAccordion({overview} : Props){

    const [response, setResponse] = useState<string>();
    const [responseStatus, setResponseStatus] = useState<number>();

    // DEPRECATED
    async function addCompanyToUserProfile()
    {
        axios.post(`${ConnectionUtility.getServerBaseUrl()}/StocksOverview/AddCompanyToUserProfile`,
            {
                CompanySymbol: overview.symbol,
            })
            .then(async response =>
            {
                setResponseStatus(response.status)
                setResponse(response.data)
            })
            .catch(async error => {
                setResponseStatus(error.response.status)
                setResponse(error.response.data)
            })
    }

    const handleDelete = () => {
        axios.delete(`${ConnectionUtility.getServerBaseUrl()}/StocksOverview/Delete/${overview.symbol}`)
        .then(response => toast.success(response.data))
        .catch(error => toast.error(error?.response?.data ?? "An error occured"))
    }

    function getColor(score: number): string {
        if (score == null) {
            return "#808080"; // Neutral grey
        }
        if (score > 7) {
            return "#4CAF50"; // Green
        }
        if (score > 5) {
            return "#FFEB3B"; // Yellow
        }
        if (score > 0) {
            return "#F44336"; // Red
        }
        return "#808080"; //
    }

    return (
        <Accordion>
            <AccordionSummary
                expandIcon={<ExpandMoreIcon />}
                aria-controls="panel1-content"
                id="panel1-header"
            >
                <Typography variant="body1"  sx={{ mr: 0.5 }}>{overview.symbol}</Typography>:<Typography sx={{ ml: 0.5 }} variant="body1" color="text.secondary">{overview.name}</Typography>
            </AccordionSummary>
            <AccordionDetails>
                <Typography variant="body1"><b>Exchange: </b> {overview.exchange}</Typography>
                <Typography variant="body1"><b>Reputation Score: </b> {overview.reputationValue?.toString()}
                    <StarRateIcon fontSize="small" sx={{ color: getColor(overview.reputationValue) }}/>
                </Typography>
                <Typography variant="body1"><b>Reputation Facts: </b></Typography>
                <List>
                    {overview.reputationFacts?.map((fact) => (
                        <ListItem sx={{ paddingTop: 0, paddingBottom: 0, minHeight: 'auto' }}>
                        <ListItemIcon sx={{ minWidth: 'auto', marginRight: 1 }}>
                            <InfoOutlinedIcon fontSize="small" color="primary" />
                        </ListItemIcon>
                            <ListItemText
                                primary={
                                    <Typography variant="body2" fontStyle="italic" color="text.secondary" sx={{ lineHeight: 1 }}>
                                        {fact}
                                    </Typography>
                                }
                            />
                        </ListItem>
                    ))}
                </List>
                <br />
                <Typography variant="body1"><b>Description: </b>
                    <Typography variant="body2" color="text.secondary">{overview.description}</Typography>
                </Typography>
                <AccordionActions>
                    <Button color="secondary" onClick={() => handleDelete()}>Remove</Button>
                </AccordionActions>
            </AccordionDetails>
        </Accordion>
    )
}