<?php
$hostname = 'localhost';
$username = 'root';
$password = '0000';
$database = 'playerDataBase';
$secrtKey = "UnityC";

// data base 접속
try {	
	$dbh = new PDO('mysql:host='.$hostname.';dbname='.$database, $username, $password);
}

catch(PDOException $e){
	echo '<h1> An error has occureed. </h1><pre>', $e ->getMessage(), '</pre>';
}

//$hash = $_GET['hash'];

$realHash = hash('sha256',
$_GET['id'].
$_GET['pw']);

	// query 문
	$sth = $dbh->
	prepare('INSERT INTO playerlogin VALUES (
		:id, 
		:pw,
		null
		);');

	try
	{
		$sth -> bindParam(':id', $_GET['id'],
		PDO::PARAM_STR);
		
		$sth -> bindParam(':pw', $_GET['pw'],
		PDO::PARAM_STR);
		

	if($sth -> execute()) { echo "excute 성공!!!";}
	else{echo "excute 실패 ㅠㅠㅠ!!!";}
	}

	catch(Exception $e)
	{
	echo'<h1>An error has ocurred</h1><pre>',
	$e-> getMessage(),'</pre>';
	}
