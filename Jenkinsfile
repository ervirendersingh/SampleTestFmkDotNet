node {
    stage('Checkout git repo') {
      git branch: 'testbranch', url: params.git_repo
    }
    stage('build and publish') {
        sh(script: "dotnet publish SampleTestFmk.sln -c Release ", returnStdout: true)
    }
}