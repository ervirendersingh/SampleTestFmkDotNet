node {
    stage('Checkout git repo') {
      git branch: 'testbranch', url: params.git_repo
    }

	docker.image('node:7-alpine').inside {
        stage('Test') {
            sh 'node --version'
			sh 'cd /root'
			sh 'ls -la'
        }
    }
	
}