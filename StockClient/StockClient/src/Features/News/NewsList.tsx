import { Typography } from "@mui/material";
import ArticleList from "./ArticleList.tsx";
import { ArticlesGroup } from "../../Models/ArticlesGroup.tsx";


interface Props{
    articlesGroups: ArticlesGroup[];
}

export default function NewsList({ articlesGroups }: Props) {
    if (articlesGroups?.length > 0) {
        return (
            <>
                {articlesGroups.map(group => (
                    <>
                        <Typography key={group.groupName} variant="h6" color="text.primary">{group.groupName}</Typography>
                        <ArticleList articleList={group.articles}/>
                    </>
                ))}
            </>
        );
    }
    return null;
}