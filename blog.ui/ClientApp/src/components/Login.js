import React, { useState } from 'react'
import { Row, Col, Button, Form, Input, Label } from 'reactstrap'
import { useHistory } from 'react-router-dom'
import { setSession } from '../utils/Sessions';

const Login = () => {
    const [email, setEmail] = useState();
    const [password, setPassword] = useState();
    const history = useHistory()

    const handleSubmit = async (e) => {
        e.preventDefault()
        const response = await fetch('/api/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ email: email, password: password })
        })
        if (response.ok) {
            const json = await response.json()
            setSession(json)
            history.push('/new')
        }
    }

    return (
        <Row>
            <Col xs={0} sm={0} lg={3} />
            <Col>
                <Form onSubmit={handleSubmit}>
                    <Label for='email'>Email</Label>
                    <Input name='email' id='email' required type='email' onChange={e => setEmail(e.target.value)} />
                    <Label for='password'>Password</Label>
                    <Input name='password' id='password' required type='password' onChange={e => setPassword(e.target.value)} />
                    <Button type='submit' color='primary' title='Login'>Login</Button>
                </Form>
            </Col>
            <Col xs={0} sm={0} lg={3} />
        </Row>
        )
}

export default Login