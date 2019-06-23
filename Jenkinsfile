node {
    stage('Checkout git repo') {
      git branch: 'testbranch', url: params.git_repo
    }

	
	docker.image('node:7-alpine').inside('-v $HOME:/root/') {
        stage('Test') {
            sh 'node --version'
			sh 'cd /root'
			sh 'ls -la'
        }
    }
}