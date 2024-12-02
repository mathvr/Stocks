import {Box, Button, Container, FormControl, Grid, TextField, Typography} from "@mui/material";
import {useState} from "react";
import axios from "axios";
import { ConnectionUtility } from "../../App/Utility/ConnectionUtility";

export default function Login()
{
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [loginResponse, setLoginResponse] = useState("");
    const [loginResponseStatus, setLoginResponseStatus] = useState<number>();
    const [registerResponse, setRegisterResponse] = useState("");
    const [registerResponseStatus, setRegisterResponseStatus] = useState<number>()
    async function submitRegisterForm()
    {
        axios.post(`${ConnectionUtility.getServerBaseUrl()}/AppUser/AddAppUser`,
            {
                UserNumber: email,
                FirstName: firstName,
                LastName: lastName,
                Password: password,
                AccessLevel: "WebsiteUser"
            })
            .then(response =>
            {
                setRegisterResponse(response.data)
                setRegisterResponseStatus(response.status)
            })
            .catch(error => {
                setRegisterResponseStatus(error.response.status)
                setRegisterResponse(error.response.data)
            })
    }

    async function Login()
    {
        axios.post(`${ConnectionUtility.getServerBaseUrl()}/AppUser/Login`,
            {
                email: email,
                password: password,
            })
            .then(response =>
            {
                setLoginResponse(response.data)
                setLoginResponseStatus(response.status)
            })
            .catch(error => {
                setLoginResponseStatus(error.response.status)
                setLoginResponse(error.response.data)
            })
    }

    return(
        <Container>
            <Grid container spacing={2}>
                <Grid xs={6} padding={4}>
                    <Typography variant="h2" color="text.secondary" align="center" sx={{ mb: 6 }}>Login</Typography>
                    <FormControl fullWidth="true">
                        <TextField
                            sx={{ mb: 1 }}
                            email
                            id="outlined-required"
                            label="Email"
                            onChange={e => setEmail(e.target.value)}
                        />
                        <TextField
                            sx={{ mb: 1 }}
                            password
                            id="outlined-required"
                            label="Password"
                            onChange={e => setPassword(e.target.value)}
                        />
                        <Button variant="outlined" onClick={Login}>Submit</Button>
                        <Typography
                            sx ={{mt: 2}}
                            variant="body2"
                            color= {loginResponseStatus == 200 ? '#28a745' : '#ff0000'}
                        >{loginResponse}</Typography>
                    </FormControl>
                </Grid>
                <Grid xs={6} padding={4}>
                    <Typography variant="h2" color="text.secondary" align="center" sx={{ mb: 6 }}>Register</Typography>
                    <FormControl fullWidth="true">
                        <TextField
                            sx={{ mb: 1 }}
                            first name
                            id="outlined-required"
                            label="First name"
                            defaultValue=""
                            onChange={e => setFirstName(e.target.value)}
                        />
                        <TextField
                            sx={{ mb: 1 }}
                            last name
                            id="outlined-required"
                            label="Last name"
                            defaultValue=""
                            onChange={e => setLastName(e.target.value)}
                        />
                        <TextField
                            sx={{ mb: 1 }}
                            email
                            id="outlined-required"
                            label="Email"
                            defaultValue=""
                            onChange={e => setEmail(e.target.value)}
                        />
                        <TextField
                            sx={{ mb: 1 }}
                            password
                            id="outlined-required"
                            label="Password"
                            defaultValue=""
                            onChange={e => setPassword(e.target.value)}
                        />
                        <Button variant="outlined" onClick={submitRegisterForm}>Submit</Button>
                        <Typography
                            sx ={{mt: 2}}
                            variant="body2"
                            color= {registerResponseStatus == 200 ? '#28a745' : '#ff0000'}
                        >{registerResponse}</Typography>
                    </FormControl>
                </Grid>
            </Grid>
        </Container>
    )
}