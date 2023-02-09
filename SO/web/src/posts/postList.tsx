import * as React from 'react';
import Grid from '@mui/material/Grid';
import Container from '@mui/material/Container';
import { useEffect } from 'react';
import { Api } from '../apiClient/Api';
import { PaginatedPostList, PostListDto } from '../apiClient/data-contracts';
import toast from 'react-hot-toast';
import PostCard from './postCard';

export default function PostList() {
    const [posts, setPosts] = React.useState<PostListDto[]>([]);
    useEffect(() => {
        const api = new Api({
            baseUrl: "http://localhost:5000"
        });

        const fetchData = async () => {
            const posts = await api.postList({ Offset: 0, Limit: 20 });
            if (posts.ok) {
                setPosts(posts.data.posts ?? []);
            } else {
                toast("Cannot get posts");
            }
        }
        fetchData()
            .catch(() => toast("Cannot get posts"));
    }, []);

    return (
        <Container sx={{ py: 2 }} >
            <Grid container spacing={4}>
                {posts.map((post) => (
                    <PostCard post={post}/>
                ))}
            </Grid>
        </Container>
    );
}
