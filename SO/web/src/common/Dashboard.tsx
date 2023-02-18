import { createTheme, ThemeProvider } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Container from '@mui/material/Container';
import Grid from '@mui/material/Grid';
import Paper from '@mui/material/Paper';
import NewPosts from '../posts/newPosts';
import Footer from './Footer';
import { Toaster } from 'react-hot-toast';
import SideBarMenu from './SideBarMenu';
import PostList from '../posts/postList';


const mdTheme = createTheme();

function DashboardContent() {
    return (
        <ThemeProvider theme={mdTheme}>
            <div><Toaster 
                position="top-right"
                reverseOrder={false}/>
            </div>
            <Box sx={{ display: 'flex' }}>
                <CssBaseline />
                <SideBarMenu/>
                <Box
                    component="main"
                    sx={{
                        backgroundColor: (theme) =>
                            theme.palette.mode === 'light'
                                ? theme.palette.grey[100]
                                : theme.palette.grey[900],
                        flexGrow: 1,
                        height: '100vh',
                        overflow: 'auto',
                    }}
                >
                    <Toolbar />
                    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
                        <Grid container spacing={3}>
                            <Grid item xs={12} md={8} lg={9}>
                                <Paper
                                    sx={{
                                        p: 2,
                                        display: 'flex',
                                        flexDirection: 'column',
                                    }}
                                >
                                    <NewPosts />
                                </Paper>
                            </Grid>
                            <Grid item xs={12} md={4} lg={3}>
                                <Paper
                                    sx={{
                                        p: 2,
                                        display: 'flex',
                                        flexDirection: 'column',
                                        height: 240,
                                    }}
                                >
                                    Featured users //TODO
                                </Paper>
                            </Grid>
                            <Grid item xs={12}>
                                <Paper sx={{ p: 2, display: 'flex', flexDirection: 'column' }}>
                                    <PostList/>
                                </Paper>
                            </Grid>
                        </Grid>
                        <Footer />
                    </Container>
                </Box>
            </Box>
        </ThemeProvider>
    );
}

export default function Dashboard() {
    return <DashboardContent />;
}