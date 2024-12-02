import {StockOverview} from "../../Models/StockOverview.tsx";
import React, {Fragment} from "react";
import {Accordion, Grid, List, ListItem, Typography} from "@mui/material";
import StockOverviewCard from "./StockOverviewAccordion.tsx";
import StockOverviewAccordion from "./StockOverviewAccordion.tsx";

interface Props{
    overviews: StockOverview[];
}
export default function StockOverviewList({overviews}: Props){
    if(overviews.length > 0)
    {
        return(
            <>
                {overviews?.map(overview => (
                    <StockOverviewAccordion key={overview.symbol} overview={overview} />
                ))}
            </>
        )
    }
}