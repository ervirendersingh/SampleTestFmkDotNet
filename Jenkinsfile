node {
	stage 'Checkout'{
		checkout scm
	}

	stage 'Build'{
		bat 'nuget restore SampleTestFmk.sln'
		bat "\"${tool 'MSBuild'}\" SampleTestFmk.sln /p:Configuration=Debug /p:Platform=\"Any CPU\" /p:ProductVersion=1.0.0.${env.BUILD_NUMBER}"
	}
	stage 'Test'{
		bat './runtests.bat'
	}	

	stage 'push results to gcs' {
            // After build, always upload local object named PATTERN to GCS bucket.
           googleStorageUpload bucket: 'gs://autotestresults', credentialsId: 'acdc', pattern: 'TestResult.xml'
    }

	stage 'publish results' {
            // After build, always upload local object named PATTERN to GCS bucket.
           googleStorageUpload bucket: 'gs://autotestresults', credentialsId: 'acdc', pattern: 'TestResult.xml'
    }
}