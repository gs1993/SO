import * as React from 'react';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Grid from '@mui/material/Grid';
import Container from '@mui/material/Container';
import { createTheme, styled } from '@mui/material/styles';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { Avatar, Badge, CardHeader, Collapse, IconButton, IconButtonProps, Stack } from '@mui/material';
import FavoriteIcon from '@mui/icons-material/Favorite';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import { red } from '@mui/material/colors';
import ShareIcon from '@mui/icons-material/Share';
import CommentIcon from '@mui/icons-material/Comment';
import Chip from '@mui/material/Chip';


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
    const handleExpandClick = () => {
        setExpanded(!expanded);
    };

    return (
        <Container sx={{ py: 2 }} >
            <Grid container spacing={4}>
                {cards.map((card) => (
                    <Grid item key={card} sx={{ width: '100%' }}>
                        <Card>
                            <CardHeader
                                avatar={
                                    <Avatar sx={{ bgcolor: red[500] }} aria-label="recipe">
                                        R
                                    </Avatar>
                                }
                                action={
                                    <IconButton aria-label="settings">
                                        <MoreVertIcon />
                                    </IconButton>
                                }
                                title="Monitoring the Full Disclosure mailinglist"
                                subheader="September 14, 2016"
                            />
                            <CardContent>
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
                            <Badge badgeContent={1} color="primary" max={99}>
                                <CommentIcon color="action" />
                            </Badge>
                            <Badge badgeContent={1} color="primary" max={99}>
                                <CommentIcon color="action" />
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
                                    <p>I develop web applications, which use a number of third party applications/code/services.</p><p>As part of the job, we regularly check with the Full Disclosure mailing list  for any of the products we use.</p><p>This is a slow process to do manually and subscribing to the list would cost even more time, as most reports do not concern us.</p><p>Since I can't be the only one trying to keep up with any possible problems in the code I use, others have surely encountered (and hopefully solved) this problem before.</p><p>What is the best way to monitor the Full Disclosure mailing list for specific products only?</p>
                                    </div>
                                </CardContent>
                            </Collapse>
                        </Card>
                    </Grid>
                ))}
            </Grid>
        </Container>
    );
}