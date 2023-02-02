import * as React from 'react';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Grid from '@mui/material/Grid';
import { styled } from '@mui/material/styles';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { Avatar, Badge, Button, CardHeader, Collapse, IconButton, IconButtonProps, Stack } from '@mui/material';
import { blue } from '@mui/material/colors';
import CommentIcon from '@mui/icons-material/Comment';
import Chip from '@mui/material/Chip';
import VisibilityIcon from '@mui/icons-material/Visibility';
import StarBorderIcon from '@mui/icons-material/StarBorder';
import FiberNewIcon from '@mui/icons-material/FiberNew';
import { PostListDto } from '../apiClient/data-contracts';
import { format } from 'date-fns';

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

interface PostCardProps {
    post: PostListDto;
}
export default function PostCard(props: PostCardProps) {
    const [expanded, setExpanded] = React.useState(false);
    const post = props.post;

    const handleExpandClick = () => {
        setExpanded(!expanded);
    };

    return (
        <Grid item key={post.id} sx={{ width: '100%' }}>
            <Card>
                <CardHeader
                    avatar={
                        <Avatar sx={{ bgcolor: blue[200] }} aria-label="recipe">
                            {post.userName 
                                ? post.userName?.substring(0, 1)?.toLocaleUpperCase() 
                                : 'U'}
                        </Avatar>
                    }
                    action={
                        <FiberNewIcon sx={{ fontSize: 50 }} color="error" />
                    }
                    title={post.title}
                    subheader={format(new Date(post.creationDate ?? ""), 'MMMM dd, yyyy')}
                />
                <CardContent>
                    <Stack direction="row" spacing={1}>
                        {post.tags?.map((tag) => (
                            <Chip label={tag} />
                        ))}
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
                                {post.body}
                            </div>
                        </CardContent>
                        <CardActions>
                            <Button size="small">Share</Button>
                            <Button size="small">More info</Button>
                        </CardActions>
                    </Collapse>
            </Card>
        </Grid>
    )
}