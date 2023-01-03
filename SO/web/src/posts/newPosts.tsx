import * as React from 'react';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Grid from '@mui/material/Grid';
import Container from '@mui/material/Container';
import { createTheme, styled } from '@mui/material/styles';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { Avatar, Badge, Button, CardHeader, Collapse, IconButton, IconButtonProps, Stack } from '@mui/material';
import { blue } from '@mui/material/colors';
import CommentIcon from '@mui/icons-material/Comment';
import Chip from '@mui/material/Chip';
import VisibilityIcon from '@mui/icons-material/Visibility';
import StarBorderIcon from '@mui/icons-material/StarBorder';
import FiberNewIcon from '@mui/icons-material/FiberNew';
import { useEffect } from 'react';
import { Api } from '../apiClient/Api';
import { PostListDto } from '../apiClient/data-contracts';
import MuiAlert, { AlertProps } from '@mui/material/Alert';
import toast from 'react-hot-toast';
import { format } from 'date-fns';

const Alert = React.forwardRef<HTMLDivElement, AlertProps>(function Alert(
    props,
    ref,
  ) {
    return <MuiAlert elevation={6} ref={ref} variant="filled" {...props} />;
  });
  



interface ExpandMoreProps extends IconButtonProps {
    expand: boolean;
}
const ExpandMore = styled((props: ExpandMoreProps) => {
    const { expand, ...other } = props;
    return <IconButton {...other} />;
})(({ theme, expand }) => ({
    transform: !expand ? 'rotate(0deg)' : 'rotate(180deg)',
    marginLeft: 'auto',
    transition: theme.transitions.create('transform', {
        duration: theme.transitions.duration.shortest,
    })
}));

const cards = [1, 2, 3];

const theme = createTheme();

export default function NewPosts() {
    const [expanded, setExpanded] = React.useState(false);
    const [posts, setPosts] = React.useState<PostListDto[]>([]);

    useEffect(() => {
        const api = new Api({
            baseUrl: "http://localhost:5000"
        });
    
        const fetchData = async () => {
            const posts = await api.postGetLastestList({ Size: 3 });
            if (posts.ok) {
                setPosts(posts.data);
            } else {
                toast("Cannot get posts");
            }
        }
        fetchData()
            .catch(() => toast("Cannot get posts"));
    }, []);
    

    const handleExpandClick = () => {
        setExpanded(!expanded);
    };

    return (
        <Container sx={{ py: 2 }} >
            <Grid container spacing={4}>
                {posts.map((post) => (
                    <Grid item key={post.id} sx={{ width: '100%' }}>
                        <Card>
                            <CardHeader
                                avatar={
                                    <Avatar sx={{ bgcolor: blue[200] }} aria-label="recipe">
                                        R
                                    </Avatar>
                                }
                                action={
                                    <FiberNewIcon sx={{ fontSize: 50 }} color="error" />
                                }
                                title={post.title}
                                subheader={format(new Date(post.creationDate ?? ""), 'MMMM dd, yyyy')}
                            />
                            <CardContent>
                            {/* TODO: add tags */}
                                <Stack direction="row" spacing={1}>
                                    <Chip label="security" />
                                    <Chip label="email" />
                                    <Chip label="monitoring" />
                                    <Chip label="filter" />
                                    <Chip label="rss" />
                                </Stack>

                            </CardContent>
                            <CardActions disableSpacing>
                                <Stack direction="row" spacing={2}>
                                    <Badge badgeContent={post.viewCount ?? 0} color="primary" max={99}>
                                        <VisibilityIcon color="action" />
                                    </Badge>
                                    <Badge badgeContent={post.commentCount ?? 0} color="primary" max={99}>
                                        <CommentIcon color="action" />
                                    </Badge>
                                    <Badge badgeContent={post.score ?? 0} color="primary" max={99}>
                                        <StarBorderIcon color="action" />
                                    </Badge>

                                </Stack>
                                <ExpandMore
                                    expand={expanded}
                                    onClick={handleExpandClick}
                                    aria-expanded={expanded}
                                    aria-label="show more"
                                >
                                    <ExpandMoreIcon />
                                </ExpandMore>
                            </CardActions>
                            <Collapse in={expanded} timeout="auto" unmountOnExit>
                                <CardContent>
                                    <div>
                                        {post.shortBody}
                                    </div>
                                </CardContent>
                                <CardActions>
                                    <Button size="small">Share</Button>
                                    <Button size="small">More info</Button>
                                </CardActions>
                            </Collapse>
                        </Card>
                    </Grid>
                ))}
            </Grid>
        </Container>
    );
}

function useState(arg0: never[]): [any, any] {
    throw new Error('Function not implemented.');
}
