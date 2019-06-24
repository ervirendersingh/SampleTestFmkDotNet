node {
	stage('Checkout git repo') {
      git branch: 'testbranch', url: params.git_repo
    }

	stage('Build'){
		bat 'nuget restore SampleTestFmk.sln'
		bat "\"${tool 'MSBuild'}\" SampleTestFmk.sln /p:Configuration=Debug /p:Platform=\"Any CPU\" /p:ProductVersion=1.0.0.${env.BUILD_NUMBER}"
	}
	stage('Test'){
		bat './runtests.bat'
	}	

	stage ('Zip Results and push to gcs') {
        zip zipFile: 'results.zip', archive: false, dir: 'Results'
        archiveArtifacts artifacts: 'results.zip', fingerprint: true
		googleStorageUpload bucket: 'gs://autotestresults', credentialsId: 'acdc', pattern: 'results.zip'
            
    }

	stage('publish results'){        
        nunit testResultsPattern: './Results/TestResult.xml'
    }
}