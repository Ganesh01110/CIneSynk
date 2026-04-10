output "alb_dns_name" {
  description = "The DNS name of the load balancer"
  value       = aws_lb.main.dns_name
}

output "rds_endpoint" {
  description = "The connection endpoint for the RDS instance"
  value       = aws_db_instance.main.endpoint
}

output "ecr_repository_url_api" {
  description = "The URL of the API ECR repository"
  value       = aws_ecr_repository.api.repository_url
}

output "ecr_repository_url_frontend" {
  description = "The URL of the Frontend ECR repository"
  value       = aws_ecr_repository.frontend.repository_url
}
