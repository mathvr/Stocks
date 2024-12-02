import { Box, Link, Typography } from "@mui/material";
import { ArticlesGroup } from "../../Models/ArticlesGroup";
import { Article } from "../../Models/Article";


interface Props{
    articleList: Article[];
}

const GetDate = (fullDate: string) => {

}

export default function ArticleList({ articleList }: Props) {


    if (articleList?.length > 0) {
        return (
            <Box sx={{ mb: 1.5 }}>
                <ul>
                    {articleList
                    .map((article) => (
                        <div key={article.id}>
                            <li><Typography>{article.publicationDateText} - <Link href={article.url}>{article.title}</Link></Typography></li>
                        </div>
                    ))}
                </ul>
            </Box>
        );
    }
    return null;
}