## Introduction

Trocitos restaurant was developed originally as a college project to showcase learning in relation to cloud infrastructure, virtualisation, application deployment and CI/CD pipelines. The assignment was based on a scenario whereby a continuous integration system was needed for a full-stack development team. The following requirements were presented:

1. Repository - A code repository that developers could access to pull and push code as required.
2. Build Pipeline - A CI/CD pipeline that developers could access to view and execute deployment of projects.
3. Database Server - A database that could be securely accessed by the development team.
4. Web Application - A web application with backend services to demonstrate the operation of the CI/CD pipeline
5. VM's set up for deployment of code and accessed via a Virtual Private Network.



## Technologies Used
- .NET 6 (ASP.NET, MVC)
- xUnit
- Moq
- Git / GitHub
- AWS (EC2, RDS, VPC, IAM, CodePipeline, Elastic Beanstalk)
- OpenVPN

## Design and Planning

### Application

With these requirements in mind I developed the concept for the application itself based on the below scenario:

*The owners of Trocitos restaurant have consulted our software development company to create a 
web-based reservation booking system for their business.*


*The immediate goal for Trocitos is to create an online presence for their well-established business 
and to improve their current method of managing reservations. They have found the current method 
of managing reservations over the phone to be too cumbersome and out-dated. The demonstrated 
development window was therefore intended to have the website operational and available for users 
to make reservations, with the intention of potentially adding more features over time. The purpose 
of this project was to design and implement a CI/CD system for a development team carrying out the 
development of the current request.*

The web application was therefore developed to a point of functonality, to demonstrate 
deployment to the test environment. The currently deployed application will allow a user to check 
availability for a reservation, book the reservation and cancel any reservation. The result of these 
transactions can be viewed in the database.

### AWS 

#### AWS CodePipeline Flow

![AWS Code Pipeline](/trocitos.mvc/wwwroot/images/code-pipeline-flow-diagram.jpg)

**The end-to-end process of the CI/CD pipeline is outlined below:**
1. User pushes code changes from local Git repository using the “git push” command.
2. This triggers the Source step of the AWS CodePipeline through a webhook.
3. A source artifact is generated and stored in the S3 bucket.
4. The source artifact is then used to trigger the build process. The build process is then 
executed as per the buildspec.yml file instructions. Both building and testing take place 
during this stage.
5. Once the build process has been successfully executed and completed, a build artifact is 
generated.
6. The build artefact is then used to trigger AWS CodeDeploy. AWS CodeDeploy then deploys 
the application to the Elastic Beanstalk environment.
7. The Elastic Beanstalk then runs an environment update process which identifies the updated 
application and deploys it to the running EC2 instance.
8. Elastic Beanstalk will then confirm that the environment update has been completed 
successfully and ensures updated changes are available in the application development 
environment.

#### Build and Test Reports
The pipeline provides build and test reports which developers for the project would be able to use during the development stages. All reports are available via the AWS developer profile. 

A test application was set up using xUnit and Moq to 
perform and demonstrate six different unit tests on the Reservation controller logic. Test reports would then be available to developers to review

![Test report](/trocitos.mvc/wwwroot/images/test-report.jpg)

#### VPC Architecture

![VPC Architecture](/trocitos.mvc/wwwroot/images/vpc-architecture.jpg)

**The VPC architecture consists of:**

- Public Subnet
  - t3.micro to host the MVC application (with elastic IP)
  - t2.micro to host the VPN server (with elastic IP)
  - Internet gateway
- Private Subnets
  - RDS running instance (eu-west-1a)
  - RDS instance failover (eu-west-1b)
  - RDS instance failover (eu-west-1c)


All access is controlled by security groups. At present the OpenVPN server must be used to access 
the RDS instance. For the original project, the elastic IP of the running EC2 instance was also only accessible via the VPN server. This has been changed however to allow incoming HTTP connections from any source. The reason for this that I'm now making the GitHub repo publicly accessible and so the application is now similarly accessible for demonstration purposes.  

For best practice, the RDS instance is located in private subnets. It will only allow communication 
from the Application or VPN security groups. The provision of three subnets in different availability 
zones is to provide a failover if one availability zone has any service interruptions, or if the RDS 
instance itself encounters any issues. This was recommended during setup of the Elastic Beanstalk 
environment. There will only be one instance of the RDS running at any one time, but this could be in 
either of the availability zones. 

Cost factors and considerations primarily influenced the decision to deploy the test application on a 
single EC2 and RDS instance. The intention was to remain (mostly) within the AWS free tier, so this 
approach was seen as best for the purpose of the project. For cost-efficiency and convenience of 
access, I chose to run fewer EC2 instances continuously, steering clear of NAT and unused Elastic IP 
fees. A NAT gateway should be applied in a real operational environment, however, for the purpose 
of this project and minimising costs, it didn’t seem necessary.


### Database

The database used for this project is an AWS RDS instance running MySQL. The current database 
schema is set to accommodate reservations being booked or cancelled and to assign a table to each 
reservation. 

There are therefore just two entities in the current schema to accommodate the current 
requirements. Further entities can be added during future development as required. Entity 
Framework Core is employed to update the database schema via migrations.

![Database Schema](/trocitos.mvc/wwwroot/images/db-schema.jpg)


### Access

#### Access to the Application

To view and use the Trocitos restaurant booking application you can navigate to either 52.210.146.37 or http://trocitos-restaurant.eu-west-1.elasticbeanstalk.com/

Previously these could only be accessed when logged into the VPN. This has now been changed so that the application can be viewed publicly. 

> **Note: There are currently no SSL certs or DNS settings setup for the site. The project was initially just to have the application deployed to a test environment with limited access. Access is now provided publicly purely for demonstration purposes. 

#### GitHub

For the purpose of the project I created a GitHub organisation called "dublin-devs". This was to allow for the repo to be kept private but still have developers, in the given scenario, access the repo to pull and push code. I was able to give access to college lecturers by sending an invite to their GitHub handle. This part of the project is now defunct as I've migrated the code back to my own GitHub profile as a public repository.

#### AWS Console Access

A developer group called Devs and a developer profile was set up on my AWS account with the appropriate IAM permissions in order to view the EC2 instances, RDS, Elastic Beanstalk Environment, CodePipeline, Build and Test reports and VPC. The username and password was provided for the project submission but has not been included here.

![AWS Dev Permissions](/trocitos.mvc/wwwroot/images/dev-permissions.jpg)

#### Open VPN Access

Open VPN Access was provided via an elastic ip address. By navigating to https://52.214.216.107, downloading the VPN client and logging in to the provided profile, developers would then have access to the application and RDS. Again, the password has not been provided as part of this readme but was for the original project.

#### Database Access

The database can be accessed by logging into the VPN and using MySQL Workbench. Again the credentials are not provided here but were for the original project. This is to keep the AWS endpoint for the database private.

