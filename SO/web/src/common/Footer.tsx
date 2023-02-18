import { Link, Typography } from "@mui/material";

function FooterCopyright(props: any) {
    return (
        <Typography sx={{ pt: 4 }} variant="body2" color="text.secondary" align="center" {...props}>
            {'Copyright © '}
            <Link color="inherit" href="https://github.com/gs1993/">
                Grzegorz Sawiński
            </Link>{' '}
            {new Date().getFullYear()}
            {'.'}
        </Typography>
    );
}

export default function Footer() {
    return <FooterCopyright />;
}