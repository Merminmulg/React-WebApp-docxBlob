import React, { Component } from 'react';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = {
            file: null,
            email: '',
            loading: false,
            result: null,
            error: null,
        };
    }

    handleSubmit = async (event) => { 
        this.setState({ loading: true })
        event.preventDefault();

        const email = event.target.email.value;

        const emailPattern = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
        if (!emailPattern.test(email)) {
            this.setState({ loading: false, error: 'Invalid email format', result: null });
            console.error('Invalid email format');
            return;
        }

        const formData = new FormData();
        formData.append('file', event.target.file.files[0]);

        try {
            const response = await fetch('api/files/uploadfile?email=' + event.target.email.value, {
                method: 'POST',
                body: formData,
            });
            console.log(response)
            if (response.ok) {
                this.setState({ loading: false})
                this.setState({ result: response.status, error: null });
                console.log('File uploaded');
            } else {
                this.setState({ loading: false })
                this.setState({ error: 'Problem with uploading file', result: null });
                console.error('Uploading error');
            }
        } catch (ex) {
            this.setState({ loading: false })
            this.setState({ error: 'Network error', result: null });
            console.error('Network error:', ex);
        }
    }

    render() {
        return (
            <div>
                <form onSubmit={this.handleSubmit} encType="multipart/form-data">
                    <input type="file" name="file" />
                    <input type="text" name="email" />
                    <input type="submit" value="Submit" />
                </form>
                {this.state.loading && <p>Loading...</p>}
                {this.state.result && <p>File successfuly uploaded. Result: {this.state.result}</p>}
                {this.state.error && <p>Error: {this.state.error}</p>}
            </div>
        )
    }
}
